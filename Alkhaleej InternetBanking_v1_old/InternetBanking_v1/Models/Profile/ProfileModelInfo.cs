using System.Dynamic;

namespace InternetBanking_v1.Models.Profile
{
    public class ProfileModelInfo
    {
        public int CustomerProfileId  { get; set; }
        public string AvatarUrl  {get; set;}
        public string Username { get; set; }
        //public string ProfileTitle  { get; set; }
        public string name { get; set; }
        public string CurrentIP { get; set; }
        public string LastSuccessfulLogin  {get; set;}
        public string LastUnsuccessfulLogin { get; set; }
        public string PrimaryBalance { get; set; }
        public string Lang { get; set; }
  


        
    }
}