namespace SCore.Utils
{
    public class DateTimeFormat
    {
        static public string SecondsToTime(int _seconds)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(_seconds);

            string answer = string.Format("{0:D2}:{1:D2}",
                            t.Minutes,
                            t.Seconds);

            return answer;
        }

        static public string SecondsToTimeHMS(int _seconds)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(_seconds);

            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            return answer;
        }
    }
}