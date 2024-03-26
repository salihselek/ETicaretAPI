using ETicaretAPI.Application.Enums;

namespace ETicaretAPI.Application.CustomAttributes
{
    public class AuthorizeDefinationAttribute : Attribute
    {
        public string Menu { get; set; }
        public string Defination { get; set; }
        public ActionType ActionType { get; set; }
    }
}