using System.Text.RegularExpressions;

namespace GLPortal.Core.Settings;

public class GitLabSettings
{
    /// <summary>
    /// Default section name of the application settings used to retrieve
    /// settings
    /// </summary>
    public const string DefaultSectionName = "GitLab";
    private string priorityLabelRegex = null!;
    private string customerLabelRegex = null!;

    /// <summary>
    /// Base url of the GitLab installation
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// Token used to authenticate with GitLab.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Indicates the regular expression used to identify customer labels in GitLab.
    /// It must contain a group named "customer" that will be used to extract the customer value.
    /// </summary>
    public required string CustomerLabelRegex
    {
        get => customerLabelRegex;
        set
        {
            customerLabelRegex = value;
            CustomerLabelRegexInstance = new(value);
        }
    }

    public Regex CustomerLabelRegexInstance { get; private set; } = null!;

    /// <summary>
    /// Indicates the regular expression used to identify priority labels in GitLab.
    /// It must contain a group named "priority" that will be used to extract the priority value.
    /// </summary>
    public required string PriorityLabelRegex
    {
        get => priorityLabelRegex; 
        set
        {
            priorityLabelRegex = value;
            PriorityLabelRegexInstance = new(value);
        }
    }

    public Regex PriorityLabelRegexInstance { get; private set; } = null!;

    /// <summary>
    /// Indicates if the alphabetical order of the priority labels is ascending or descending.
    /// Descending order is the default, this means that the label with the highest priority is the first one (e.g Priority: 1 High).
    /// </summary>
    public bool IsPriorityAscending { get; set; } = false;

    /// <summary>
    /// The ids of the projects to manage in GLPortal.
    /// </summary>
    public required List<int> ProjectIds { get; set; }
}
