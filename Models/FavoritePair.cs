using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace two_realities.Models
{
    public class FavoritePair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? UserId { get; set; }

        public string? TitleOne   { get; set; }
        public int? YearOne  { get; set; }
        public string? TitleTwo { get; set; }
        public int? YearTwo { get; set; }    
    }
}
