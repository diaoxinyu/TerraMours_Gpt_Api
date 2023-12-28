using Newtonsoft.Json;
using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Terramours_Gpt_Vector.Dto;

namespace Terramours_Gpt_Vector.Entities
{
    public class VectorItem
    {
        [Key]
        public int VectorId { get; set; }
        public string Id { get; set; }
        [Column(TypeName = "vector(1536)")]
        public Vector? Embedding { get; set; }

        public SparseVector? SparseValues { get; set; }
        [NotMapped] // 不映射至数据库
        public MetadataMap? Metadata { get; set; }

        [Column("Metadata", TypeName = "jsonb")]
        public string? MetadataJson
        {
            get => Metadata != null ? JsonConvert.SerializeObject(Metadata) : null;
            set => Metadata = !string.IsNullOrEmpty(value) ? JsonConvert.DeserializeObject<MetadataMap>(value) : null;
        }

        public string IndexName { get; set; }
        public string Namespace { get; set; }
    }
}
