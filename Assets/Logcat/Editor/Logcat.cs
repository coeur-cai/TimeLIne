using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor;
using UnityEngine;
public static class Logcat
{
    private static string newLogPdbFolderPath = "";
    private static string baseDir = Application.dataPath;
    private static string searchPath = "";

    //要屏蔽的文件路径(默认在Assets下)
    private static string[] ignorePaths =
    {
    };

    [MenuItem("Tools/程序工具/高性能数字化日志/生成LogPdb")]
    private static void StartInsert()
    {
        //设置新生成的LogPdb路径
        newLogPdbFolderPath = Application.dataPath + "/StreamingAssets";
        if (Directory.Exists(newLogPdbFolderPath))
        {
            //设置要插入代码的文件夹路径
            //searchPath = @"\_Code\Battle\_Library";
            searchPath = @"\code\test";
            string fullPath = Application.dataPath + searchPath;
            if (Directory.Exists(fullPath))
            {
                //生成LogPdb
                GenerateLogPdb();
            }
        }
    }
    private static void GenerateLogPdb()
    {
        //搜索指定目录，得到所有匹配文件路径
        string[] subPaths = SearchFiles.SearchAllFilesWithSpecifiedSuffixs(baseDir, searchPath, ".cs,.lua");
        //新建LogPdb映射
        LogTrackPdbFile pdb = new LogTrackPdbFile();
        //处理所有子文件
        foreach (var subPath in subPaths)
        {
            //剔除所有屏蔽文件
            bool ignoreSign = false;
            foreach(var ignorePath in ignorePaths)
            {
                if (subPath == ignorePath)
                {
                    ignoreSign = true;
                    break;
                }
            }
            if (ignoreSign == false)
            {
                //编码手动插入的代码
                HashLogTrackCode(baseDir, subPath, pdb);
            }
        }
        //生成新LogPdb
        pdb.CreateLogPdb(newLogPdbFolderPath + "/newLogPdb.json");
    }

    //编码手动插入的代码
    private static void HashLogTrackCode(string baseDir, string subPath, LogTrackPdbFile pdb)
    {
        #region 正则表达式拆分
        //第1组，匹配权限标识符
        string identifier = @"(public|private|protected)*";
        //第2,3,4组，匹配修饰符(括号从左往右顺序)
        string qualifier = @"((\s+(static|override|virtual)*\s+)|\s+)";
        //第5组，匹配返回值中的泛型(注意，因为有逗号，所以不支持多个返回值，即元组)
        string genericity = @"\w+(<\w+>)*";
        //第6组，匹配返回值中的数组标记
        string arraySign = @"(\[\])*\s+";
        //第7组，匹配函数名和泛型(格式同第5组)
        //第8组，匹配函数参数列表
        string paramList = @"\s*\(([^\)]+\s*)?\)";
        //标记函数体第1个左大括号及后面的内容，直到遇到第2个左大括号
        string leftBraceAndContent = @"\s*\{[^\{\}]*";
        //第9,10,11,12,13组，以成对的方式匹配从第2个左大括号起的所有相应的右大括号，直到第2个左大括号被匹配掉
        //第11组,平衡组,匹配第2个左大括号
        //第10组，包含第11组，额外匹配第2个左大括号后的非{}字符
        //第13组,反平衡组,匹配第1个右大括号
        //第12组，包含第13组，额外匹配第1个右大括号后的非{}字符
        //第9组，包含第10,11,12,13组，匹配除第1对外的成对括号对
        string mainText = @"(((?'Open'\{)[^\{\}]*)+((?'-Open'\})[^\{\}]*)+)*";
        //第14,15,16组，检测括号不成对的情况，还有没匹配掉的Open标签则匹配失败，随后匹配掉最后一个右大括号
        string check = @"(?(Open)(?!))\}";
        #endregion
        //重置需要写入标记
        bool hasChanged = false;
        var fullPath = baseDir + subPath;
        if (!File.Exists(fullPath))
        {
            return;
        }
        var text = File.ReadAllText(fullPath);
        //识别类
        Regex regexClassAll = new Regex(identifier + qualifier + @"class\s+\w+"+ leftBraceAndContent + mainText + check);
        //识别类名
        Regex regexClassName = new Regex(@"(?<=class\s+)\w+(?=[\s\n]*{)");
        //识别函数体
        Regex regexFuncAll = new Regex(identifier + qualifier + genericity + arraySign + genericity + paramList + leftBraceAndContent + mainText + check);
        //识别函数头
        Regex regexFuncHead = new Regex(identifier + qualifier + genericity + arraySign + genericity + paramList);
        //识别函数名，左侧有空格，右侧有左括号或<>加左括号)
        Regex regexFuncName = new Regex(@"(?<=\s+)\w+?(?=\(|<\w*>\()");
        //识别左大括号
        Regex regexLeftBrace = new Regex(@"\{");
        //识别日志代码
        Regex regexLogTrackCode = new Regex(@"FSPDebuger\.LogTrack.*((;)|(\*/))");
        //识别日志代码(提取/**/间的输出内容)
        //Regex regexLogTrackCodeBetweenStars = new Regex(@"(?<=/\*)[^/\*]+(?=\*/)");
        Regex regexLogTrackCodeBetweenStars = new Regex(@"(?<=/\*).*(?=\*/)");
        //识别日志代码(提取+号)
        Regex regexLogTrackCodePlus = new Regex(@"\+");
        //识别日志代码(提取逗号)
        Regex regexLogTrackCodeComma = new Regex(@",");
        //识别日志代码(提取""间的输出内容)
        //Regex regexLogTrackCodeBetweenDoubleQuotation = new Regex(@"(?<="")[^""]+(?="")");
        Regex regexLogTrackCodeBetweenDoubleQuotation = new Regex(@"(?<="").*(?="")");

        //识别日志ID
        Regex regexLogHash = new Regex(@"(?<=\(.*?)\d+?(?=\s*[,\)])");

        //获取所有类
        var matchesClassAll = regexClassAll.Matches(text);
        //逐个类操作(至少要有一个类)
        for (int i = matchesClassAll.Count - 1; i >= 0; i--)
        {
            //获取类名
            var matchClassName = regexClassName.Match(text, matchesClassAll[i].Index, matchesClassAll[i].Length);
            //获取该类下的所有函数
            var matchesFuncAll = regexFuncAll.Matches(matchesClassAll[i].Value);
            //逐个函数操作
            for (int j = matchesFuncAll.Count - 1; j >= 0; j--)
            {
                //获取函数头
                var matchFuncHead = regexFuncHead.Match(text, matchesClassAll[i].Index + matchesFuncAll[j].Index, matchesFuncAll[j].Length);
                //获取函数名
                var matchFuncName = regexFuncName.Match(text, matchFuncHead.Index, matchFuncHead.Length);
                //匹配左大括号
                var matchLeftBrace = regexLeftBrace.Match(text, matchesClassAll[i].Index + matchesFuncAll[j].Index, matchesFuncAll[j].Length);
                //没找到左大括号，函数不完整，不打印日志
                if (matchLeftBrace.Success)
                {
                    //获取该函数下对应的日志代码
                    var matchesLogTrackCode = regexLogTrackCode.Matches(matchesFuncAll[j].Value);
                    //逐条日志代码操作
                    for (int k = matchesLogTrackCode.Count - 1; k >= 0; k--)
                    {
                        hasChanged = true;
                        //获取所有+号(输出内容中不能有+号和逗号)
                        var matchesLogTrackCodePlus = regexLogTrackCodePlus.Matches(matchesLogTrackCode[k].Value);
                        //逐个+号操作(+号操作完后不会影响Index，因为逗号也是1位，因此可以把插入/**/的操作放后面)
                        for (int m = matchesLogTrackCodePlus.Count - 1; m>=0; m--)
                        {
                            //移除+号
                            text = text.Remove(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchesLogTrackCodePlus[m].Index, 1);
                            //插入逗号
                            text = text.Insert(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchesLogTrackCodePlus[m].Index, ",");
                        }
                        //获取参数个数
                        int argsCount = matchesLogTrackCodePlus.Count;
                        //获取双引号间的输出内容
                        var matchLogTrackCodeBetweenDoubleQuotation = regexLogTrackCodeBetweenDoubleQuotation.Match(matchesLogTrackCode[k].Value);
                        //若双引号间无输出内容，说明是之前编码的日志代码，从/**/间寻找
                        if (matchLogTrackCodeBetweenDoubleQuotation.Success == false)
                        {
                            var matchLogTrackCodeBetweenStars = regexLogTrackCodeBetweenStars.Match(matchesLogTrackCode[k].Value);
                            //找到就提取输出内容
                            if (matchLogTrackCodeBetweenStars.Success == true)
                            {
                                //更新参数个数为逗号的个数
                                var matchesLogTrackCodeComma = regexLogTrackCodeComma.Matches(matchesLogTrackCode[k].Value);
                                argsCount = matchesLogTrackCodeComma.Count;
                                //构建条目并添加到LogPdb，并获取合法ID
                                ushort validID = pdb.AddItem(subPath, matchClassName.Value, matchFuncName.Value, matchLogTrackCodeBetweenStars.Value,argsCount);
                                //匹配日志ID
                                var matchLogHash = regexLogHash.Match(matchesLogTrackCode[k].Value);
                                //移除原ID
                                text = text.Remove(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchLogHash.Index, matchLogHash.Value.Length);
                                //插入新ID
                                text = text.Insert(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchLogHash.Index, validID.ToString());
                            }
                        }
                        //否则是新添加的日志代码，提取输出内容，在日志末尾创建/**/并放入其中
                        else
                        {
                            //必须/**/匹配不到才插入
                            if (regexLogTrackCodeBetweenStars.IsMatch(matchesLogTrackCode[k].Value) == false)
                            {
                                //插入输出内容到/**/
                                text = text.Insert(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchesLogTrackCode[k].Length,"/*"+ matchLogTrackCodeBetweenDoubleQuotation .Value+ "*/");
                                //构建条目并添加到LogPdb，并获取合法ID
                                ushort validID = pdb.AddItem(subPath, matchClassName.Value, matchFuncName.Value, matchLogTrackCodeBetweenDoubleQuotation.Value,argsCount);
                                //移除双引号中的输出内容
                                text = text.Remove(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchLogTrackCodeBetweenDoubleQuotation.Index - 1, matchLogTrackCodeBetweenDoubleQuotation.Value.Length + 2);
                                //添加ID实现替换
                                text = text.Insert(matchesClassAll[i].Index + matchesFuncAll[j].Index + matchesLogTrackCode[k].Index + matchLogTrackCodeBetweenDoubleQuotation.Index - 1, validID.ToString());
                            }
                        }
                    }
                }
            }
            //所有函数处理完后,若有手动插入的代码，需重新写入文件
            if (hasChanged)
            {
                File.WriteAllText(baseDir + subPath, text);
            }
        }
    }
}
