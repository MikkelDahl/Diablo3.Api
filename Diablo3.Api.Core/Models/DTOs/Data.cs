namespace Diablo3.Api.Core.Models.DTOs;

public class Data
{
   public Data()
   {
      
   }

   public Data(int level, long clearTimeInMs)
   {
       number = level;
       timestamp = clearTimeInMs;
   }
   
   public string id { get; set; }
   public string String { get; set; }
   public int number { get; set; }
   public long timestamp { get; set; }
}