using GLPortal.Core.Enums;

namespace GLPortal.Core.Models
{
    public class IssueQueryParameters
    {
        public IssueState? State { get; set; } // Esempio: "opened", "closed", "all"
        public string? Labels { get; set; } // Esempio: "bug,high priority"
        public string? Milestone { get; set; } // Nome della milestone
        public string? Search { get; set; } // Testo di ricerca nel titolo e nella descrizione
        public string? AssigneeUsername { get; set; } // Username dell'assegnatario
        public string? AuthorUsername { get; set; } // Username dell'autore
        public string? OrderBy { get; set; } = "created_at"; // Campo per ordinamento
        public DateTime? CreatedAfter { get; set; } // Data di creazione successiva
        public string? Sort { get; set; } = "desc"; // Direzione ordinamento (asc/desc)
        public int? PerPage { get; set; } // Numero di risultati per pagina
        public int? Page { get; set; } // Numero della pagina

        public override string ToString()
        {
            var parameters = new List<string>();

            if (State.HasValue) parameters.Add($"state={State.Value.ToString().ToLower()}");
            if (!string.IsNullOrEmpty(Labels)) parameters.Add($"labels={Uri.EscapeDataString(Labels)}");
            if (!string.IsNullOrEmpty(Milestone)) parameters.Add($"milestone={Uri.EscapeDataString(Milestone)}");
            if (!string.IsNullOrEmpty(Search)) parameters.Add($"search={Uri.EscapeDataString(Search)}");
            if (!string.IsNullOrEmpty(AssigneeUsername)) parameters.Add($"assignee_username={Uri.EscapeDataString(AssigneeUsername)}");
            if (!string.IsNullOrEmpty(AuthorUsername)) parameters.Add($"author_username={Uri.EscapeDataString(AuthorUsername)}");
            if (!string.IsNullOrEmpty(OrderBy)) parameters.Add($"order_by={OrderBy}");
            if (!string.IsNullOrEmpty(Sort)) parameters.Add($"sort={Sort}");
            if (CreatedAfter.HasValue) parameters.Add($"created_after={CreatedAfter.Value:O}");
            if (PerPage.HasValue) parameters.Add($"per_page={PerPage.Value}");
            if (Page.HasValue) parameters.Add($"page={Page.Value}");

            return string.Join("&", parameters);
        }
    }
}
