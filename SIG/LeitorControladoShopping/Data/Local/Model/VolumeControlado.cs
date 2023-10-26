using SQLite;

namespace LeitorControladoShopping.Data.Local.Model
{
    public class VolumeControlado
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        public string Sigla { get; set; }
        public long Volume { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public bool IsEnviado { get; set; }
    }
}
