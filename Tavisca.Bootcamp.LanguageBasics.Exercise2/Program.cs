using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace Tavisca.Bootcamp.LanguageBasics.Exercise1
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Test(new[] {"12:12:12"}, new [] { "few seconds ago" }, "12:12:12");
            Test(new[] { "23:23:23", "23:23:23" }, new[] { "59 minutes ago", "59 minutes ago" }, "00:22:23");
            Test(new[] { "00:10:10", "00:10:10" }, new[] { "59 minutes ago", "1 hours ago" }, "impossible");
            Test(new[] { "11:59:13", "11:13:23", "12:25:15" }, new[] { "few seconds ago", "46 minutes ago", "23 hours ago" }, "11:59:23");
            Console.ReadKey(true);
        }

        private static void Test(string[] postTimes, string[] showTimes, string expected)
        {
            var result = GetCurrentTime(postTimes, showTimes).Equals(expected) ? "PASS" : "FAIL";
            var postTimesCsv = string.Join(", ", postTimes);
            var showTimesCsv = string.Join(", ", showTimes);
            Console.WriteLine($"[{postTimesCsv}], [{showTimesCsv}] => {result}");
        }

       public static string GetCurrentTime(string[] exactPostTime, string[] showPostTime)
        {
            // Add your code here.
            string postTime = "impossible";
            string[] time = Regex.Split(exactPostTime[0], ":");//split the original time to obtain hr min and sec
            if (exactPostTime.Count() == 0 || showPostTime.Count() == 0)
            {
                return postTime;
            }
            if (!(IsSame(exactPostTime).Equals(IsSame(showPostTime))))//will check if exactTime is same then PostTime should be same
            {
                return postTime;
            }

            for (int i = 0; i < exactPostTime.Count(); i++)
            {
                time = Regex.Split(exactPostTime[i], ":");//split the original time to obtain hr min and sec
                if (showPostTime[i].Contains("seconds"))
                {
                    postTime = exactPostTime[i];//if few sec ago the lexicographically smaller will be the time itself
                }
                else if (showPostTime[i].Contains("minutes"))
                {
                    postTime = GetMinutes(time, showPostTime[i], postTime);

                }
                else if (showPostTime[i].Contains("hours"))
                {
                    postTime = GetHours(time, showPostTime[i], postTime);
                }
            }
            return postTime;

        }

        public static string GetMinutes(string[] time, string showPostTime, string postTime)
        {
            int givenTime = 0;
            int showTime = 0;
            String[] shownTime = Regex.Split(showPostTime, " ");//spliting the shown sentence to obtain mins
            if (int.TryParse(time[1], out givenTime) && int.TryParse(shownTime[0], out showTime))
            {
                if (((int.Parse(time[1]) + int.Parse(shownTime[0])) % 60) != 0)
                {
                    time[0] = (((int.Parse(time[1])) + 1) % 24).ToString();//if by adding mins are converted to hours i.e min>60 the increase in hrs
                    time[1] = ((int.Parse(time[1]) + int.Parse(shownTime[0])) % 60).ToString();//add remaing mins to string
                }
                else
                {
                    time[1] = ((int.Parse(time[1]) + int.Parse(shownTime[0])) % 60).ToString();
                }
                postTime = TimeFormat(time[0], time[1], time[2]);//timeformat convert time in proper format
            }
            return postTime;

        }

        public static string GetHours(string[] time, string showPostTime, string postTime)
        {
            int givenTime = 0;
            int showTime = 0;
            String[] shownTime = Regex.Split(showPostTime, " ");// to obtain hrs ago 
            if (int.TryParse(time[0], out givenTime) && int.TryParse(shownTime[0], out showTime))
            {
                if (String.IsNullOrWhiteSpace(postTime) != true)//get the latest time so that to find lexicographically smaller time
                {
                    String[] timeNow = Regex.Split(postTime, ":");
                    time[0] = (((int.Parse(time[0]) + int.Parse(shownTime[0])) % 24)).ToString();
                    postTime = TimeFormat(time[0], timeNow[1], timeNow[2]);
                }
                else
                {
                    time[0] = (((int.Parse(time[0]) + int.Parse(shownTime[0])) % 24)).ToString();
                    postTime = TimeFormat(time[0], time[1], time[2]);
                }
            }
            return postTime;
        }

        public static string TimeFormat(string hr, string min, string sec)
        {
            if (hr.Length == 1)
            {
                hr = "0" + hr;
            }
            else if (min.Length == 1)
            {
                min = "0" + min;
            }
            else if (sec.Length == 1)
            {
                sec = "0" + sec;
            }
            return hr + ":" + min + ":" + sec;
        }

        public static bool IsSame(string[] timeArray)
        {
            for (int i = 0; i < timeArray.Count(); i++)
            {
                for (int j = 1; j < timeArray.Count(); j++)
                {
                    if (timeArray[i] != timeArray[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
