using System.ComponentModel;

namespace API.Marvel.Refit.Model
{
    public enum Personagens
    {
        [Description("Sair")]
        Sair = 0,

        [Description("Captain America")]
        CapitaoAmerica = 1,

        [Description("Doctor Strange")]
        DoutorEstranho = 2,

        [Description("Iron Man")]
        IronMan = 3,

        [Description("Spider-Man")]
        SpiderMan = 4,
        
        [Description("Thor")]
        Thor = 5
    }
}