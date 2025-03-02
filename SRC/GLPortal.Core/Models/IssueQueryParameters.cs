using GLPortal.Core.Enums;

using System.Diagnostics.CodeAnalysis;

namespace GLPortal.Core.Models
{
    public class IssueQueryParameters(int projectId) : ICloneable, IParsable<IssueQueryParameters>
    {
        public int ProjectId { get; set; } = projectId;

        public IssueState? State { get; set; } // Esempio: "opened", "closed", "all"
        public string? Labels { get; set; } // Esempio: "bug,high priority"
        public string? Milestone { get; set; } // Nome della milestone
        public string? Search { get; set; } // Testo di ricerca nel titolo e nella descrizione
        public string? AssigneeUsername { get; set; } // Username dell'assegnatario
        public string? AuthorUsername { get; set; } // Username dell'autore
        public string? OrderBy { get; set; } = "created_at"; // Campo per ordinamento
        public DateTime? CreatedAfter { get; set; } // Data di creazione successiva
        public string? Sort { get; set; } = "desc"; // Direzione ordinamento (asc/desc)
        /// <summary>
        /// Number of results per page.
        /// 100 is the max accepted value.
        /// Specify -1 to retrieve all items.
        /// </summary>
        public int? PerPage { get; set; } // Numero di risultati per pagina
        public int? Page { get; set; } // Numero della pagina

        public static IssueQueryParameters Parse(string s, IFormatProvider? provider)
        {
            if (!TryParse(s, provider, out var result))
            {
                throw new FormatException("Invalid format");
            }
            return result;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out IssueQueryParameters result)
        {
            result = null;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var parameters = s.Split('&')
                .Select(s => s.Split('='))
                .ToDictionary(_ => _[0].ToLower(), _ => _[1]);
            if (!parameters.TryGetValue("projectid", out var projectIdString)
                || !int.TryParse(projectIdString, out var projectId))
            {
                throw new Exception("ProjectId is required");
            }

            var queryParameters = new IssueQueryParameters(projectId);

            foreach (var parameter in parameters)
            {
                var value = Uri.UnescapeDataString(parameter.Value);

                switch (parameter.Key)
                {
                    case "state":
                        if (Enum.TryParse<IssueState>(value, true, out var state))
                        {
                            queryParameters.State = state;
                        }
                        break;
                    case "labels":
                        queryParameters.Labels = value;
                        break;
                    case "milestone":
                        queryParameters.Milestone = value;
                        break;
                    case "search":
                        queryParameters.Search = value;
                        break;
                    case "assignee_username":
                        queryParameters.AssigneeUsername = value;
                        break;
                    case "author_username":
                        queryParameters.AuthorUsername = value;
                        break;
                    case "order_by":
                        queryParameters.OrderBy = value;
                        break;
                    case "sort":
                        queryParameters.Sort = value;
                        break;
                    case "created_after":
                        if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var createdAfter))
                        {
                            queryParameters.CreatedAfter = createdAfter;
                        }
                        break;
                    case "per_page":
                        if (int.TryParse(value, out var perPage))
                        {
                            queryParameters.PerPage = perPage;
                        }
                        break;
                    case "page":
                        if (int.TryParse(value, out var page))
                        {
                            queryParameters.Page = page;
                        }
                        break;
                    default:
                        return false;
                }
            }

            result = queryParameters;
            return true;
        }

        public object Clone()
        {
            // use deep copy to clone the object
            var clone = new IssueQueryParameters(ProjectId);
            clone.State = State;
            clone.Labels = Labels;
            clone.Milestone = Milestone;
            clone.Search = Search;
            clone.AssigneeUsername = AssigneeUsername;
            clone.AuthorUsername = AuthorUsername;
            clone.OrderBy = OrderBy;
            clone.Page = Page;
            return clone;
        }

        public IDictionary<string, object> GetActiveParameters()
        {
            var parameters = new Dictionary<string, object>();

            if (State.HasValue) parameters.Add("State", State.Value.ToString());
            if (!string.IsNullOrEmpty(Labels)) parameters.Add("Labels", Labels);
            if (!string.IsNullOrEmpty(Milestone)) parameters.Add("Milestone", Milestone);
            if (!string.IsNullOrEmpty(Search)) parameters.Add("Search", Search);
            if (!string.IsNullOrEmpty(AssigneeUsername)) parameters.Add("Assignee", AssigneeUsername);
            if (!string.IsNullOrEmpty(AuthorUsername)) parameters.Add("Author", AuthorUsername);
            if (CreatedAfter.HasValue) parameters.Add("CreatedAfter", CreatedAfter.Value);
            
            return parameters;
        }

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
