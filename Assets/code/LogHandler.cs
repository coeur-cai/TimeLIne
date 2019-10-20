using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class LogHandler{
    //整个函数的匹配模式串
   public static  string  ms_regexFuncAll = @"(public|private|protected)((\s+(static|override|virtual)*\s+)|\s+)\w+(<\w+>)*(\[\])*\s+\w+(<\w+>)*\s*\(([^\)]+\s*)?\)\s*\{[^\{\}]*(((?'Open'\{)[^\{\}]*)+((?'-Open'\})[^\{\}]*)+)*(?(Open)(?!))\}";
    //函数头的匹配模式串
    public static string ms_regexFuncHead = @"(public|private|protected)((\s+(static|override|virtual)*\s+)|\s+)\w+(<\w+>)*(\[\])*\s+\w+(<\w+>)*\s*\(([^\)]+\s*)?\)";
    //匹配函数头之后的第一个大括号模式串
    public static string ms_regexLeftBrace = @"\{";     
    //第一句代码模式串
    public static string ms_regexFirstCode = @"(\{\s*[^;]+)|(\{\s*\})"; 
    //插入的日志代码模式串
    public static string ms_regexLogTrackCode = @"\{\s*(FSPDebuger.)(LogTrack|IgnoreTrack)\(([^\)]+\s*)?\)(\})?";                                                
    //不需要日志代码模式串
    public static string ms_regexIgnoreTrackcode = @"\{\s*(FSPDebuger.)(IgnoreTrack)\(([^\)]+\s*)?\)(\})?";
    //用于记录当前ID
    public static int curId=4;    

    /// <summary>
    /// 自动插入日志代码
    /// </summary>
    /// <param name="baseDir"></param>
    /// <param name="subPath"></param>
    public static bool InsertLogtrackCode(string baseDir,string subPath)
    {        
        bool hasChanged = false;
        var fullPath = baseDir + subPath;
        if (!File.Exists(fullPath))
        {
            
            return false;
        }
        //读取指定文件的所有code
        var text = File.ReadAllText(fullPath);
        //对指定文件的code进行模式匹配
        var matches = Regex.Matches(text, ms_regexFuncAll);
        int cnt = matches.Count;
        //匹配函数头
        Regex funcheadRegex = new Regex(ms_regexFuncHead);
        //匹配第一个左括号
        Regex leftBraceRegex = new Regex(ms_regexLeftBrace);
        //匹配函数体的第一句代码
        Regex firstHeadRegex = new Regex(ms_regexFirstCode);
        //第一句代码是插入的日志
        Regex logTrackCodeRegex = new Regex(ms_regexLogTrackCode);
        //第一句是不需要插入日志代码
        Regex logIgnoreTrackCodeRegex = new Regex(ms_regexIgnoreTrackcode);
        for (int i = cnt-1; i >=0; i--)
        {
            //获得函数体的字符串
            var matchFuncAll = matches[i];
            //对指定位置和长度的函数进行函数头的匹配
            var matchFuncHead =funcheadRegex.Match(text, matchFuncAll.Index, matchFuncAll.Length);
            //对指定位置和长度的函数进行第一个大括号的匹配
            var matchLeftBrace = leftBraceRegex.Match(text, matchFuncAll.Index, matchFuncAll.Length);
            //匹配第一个括号成功
            if (matchLeftBrace.Success)
            {
                //减小查找的范围
                int len = matchFuncAll.Index + matchFuncAll.Length - (matchLeftBrace.Index + matchLeftBrace.Length);
                //匹配第一句代码
                var  matchFirstCode = firstHeadRegex.Match(text, matchLeftBrace.Index, len);
                

                if (matchFirstCode.Success)
                {
                    //判断是否是日志代码
                    if (!logTrackCodeRegex.IsMatch(matchFirstCode.Value))                    
                    {
                        //不是日志代码，插入日志代码
                        if (!logIgnoreTrackCodeRegex.IsMatch(matchFirstCode.Value))
                        {
                            string textLogCode = GetLogTrackCode(matchFuncHead.Value,fullPath);
                            //不增加文件行数（直接插入到第一个左大括号后面）
                            text = text.Insert(matchLeftBrace.Index + matchLeftBrace.Length, textLogCode);
                            hasChanged = true;
                        }
                    }
                }
            }
        }
        if (hasChanged)
        {
            File.WriteAllText(baseDir + subPath, text);
        }
        return true;
    }

    /// <summary>
    /// 根据相应函数头生成日志代码
    /// </summary>
    /// <param name="funcHead">函数头</param>
    /// <returns></returns>
    public static string GetLogTrackCode(string funcHead,string path)
    {
        curId++;
        //定义默认插入的日志代码
        string codeText = "FSPDebuger.LogTrack(0);";
        //函数名的提取        
        codeText = codeText.Replace("0", curId.ToString());
        //1.函数名提取
        string funcNameRegex = @"\w+(<\w+>)*\s*\(";
        var matchFuncName = Regex.Match(funcHead, funcNameRegex);
        string funName=null;
        if (matchFuncName.Success)
        {
            //去掉括号
             funName = matchFuncName.Value.Substring(0, matchFuncName.Value.Length - 1);           
        }

        //2.参数(值)提取(通过反括号和逗号来判断)
        string funcArgsRegex = @"\s*[_|\w|\d]+\s*[,|/)]";
        var matchFuncArgs = Regex.Matches(funcHead, funcArgsRegex);
        var argsCnt= matchFuncArgs.Count;
        if (argsCnt!=1)
        {
            for (int j = 1; j <= argsCnt; j++)
            {
                if (j==1)
                {
                    codeText = codeText.Insert(codeText.IndexOf(curId.ToString()) + 1, "," + j.ToString()+",");
                }
                else if (j == argsCnt)
                {
                    codeText = codeText.Insert(codeText.LastIndexOf(",") + 1,  j.ToString());
                }
                else
                {
                    codeText = codeText.Insert(codeText.LastIndexOf(",") + 1, j.ToString() + ",");
                }
            }
        }
        else   //只有一个参数
        {
            codeText = codeText.Insert(codeText.IndexOf(curId.ToString()) + 1, "," + argsCnt.ToString());
        }
        //进行参数坑位处理
        for (int i = 0; i < argsCnt; i++)
        {
            if (matchFuncArgs[i].Success)
            {
               var arg= matchFuncArgs[i].Value.Substring(0, matchFuncArgs[i].Value.Length - 1);
                //更新参数
               codeText=codeText.Replace((i+1).ToString(), arg);
            }            
        }

        //3.参数类型提取
        string funcArgsTypeRegex = @"((\s*[_|\w|\d]+\s*[,|/)])|^[/(]\s*[/)])";
        var matchFuncArgsType = Regex.Matches(funcHead, funcArgsTypeRegex);
        var argstype = matchFuncArgs.Count;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();        
        for (int j = 1; j <= argstype; j++)
        {              
           sb.AppendFormat("{0}&", matchFuncArgsType[j-1].Value.Substring(1, matchFuncArgsType[j-1].Value.Length - 1));                          
        }              
        int lineIndex = GetLineNum();
        //将数据添加至结构体中(用于生成LogPdb表)
       // JsonHandler.jsHandler.allData.Add(new LogPdbJson(curId, argsCnt, path, lineIndex, funName,sb.ToString()));               
        return codeText;
    }

    /// <summary>
    /// 获得函数的行号
    /// </summary>
    /// <returns></returns>
    public static int GetLineNum()
    {  
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
        return st.GetFrame(0).GetFileLineNumber();
    }
}
