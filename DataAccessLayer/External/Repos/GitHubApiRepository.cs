using DataAccessLayer.External.Models;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.External.Repos;

public class GitHubApiRepository
{
    private readonly OctokitConfigOptions _octokitConfigOptions;
    private readonly GitHubClient _gitHubClient;
    private readonly OrganizationsClient _organizationsClient;
    public GitHubApiRepository(IOptions<OctokitConfigOptions> octokitConfigOptions)
    {
        _octokitConfigOptions = octokitConfigOptions.Value;
        _gitHubClient = new(new ProductHeaderValue(_octokitConfigOptions.ProductHeaderValue))
        {
            Credentials = new Credentials(_octokitConfigOptions.GitHubToken)
        };
        _organizationsClient = new(new ApiConnection(new Connection(new ProductHeaderValue(_octokitConfigOptions.ProductHeaderValue),
            new InMemoryCredentialStore(
                new Credentials(_octokitConfigOptions.GitHubToken)))));
    }
    public Task<Repository> GetRepository(long repositoryId)
    {
        return _gitHubClient.Repository.Get(repositoryId);
    }
    public Task<Repository> GetRepository(string repositoryName)
    {
        return _gitHubClient.Repository.Get(_octokitConfigOptions.UoYTrialsOrgName, repositoryName);
    }
    public Task<IReadOnlyList<Repository>> GetAllRepositories()
    {
        return _gitHubClient.Repository.GetAllForOrg(_octokitConfigOptions.UoYTrialsOrgName);
    }
    public Task<Issue> CreateIssue(string repo, NewIssue issue)
    {
        return _gitHubClient.Issue.Create(_octokitConfigOptions.UoYTrialsOrgName, repo, issue);
    }
    public Task<Issue> CreateIssue(long repositoryId, NewIssue issue)
    {
        return _gitHubClient.Issue.Create(repositoryId, issue);
    }
    public Task<IReadOnlyList<Octokit.User>> GetOrganisationMembers()
    {
        return _organizationsClient.Member.GetAll(_octokitConfigOptions.UoYTrialsOrgName);
    }
    public Task<Issue> GetIssueByIssueNumber(long repositoryId, int issueNumber)
    {
        return _gitHubClient.Issue.Get(repositoryId, issueNumber);
    }
    public Task<IReadOnlyList<Octokit.User>> GetAllPossibleAssigneesForRepository(long repositoryId)
    {
        return _gitHubClient.Issue.Assignee.GetAllForRepository(repositoryId);
    }

    public Task<Issue> AddAssigneesToIssue(string repoName, int issueNumber, AssigneesUpdate assignees)
    {
        return _gitHubClient.Issue.Assignee.AddAssignees(_octokitConfigOptions.UoYTrialsOrgName, repoName, issueNumber,
            assignees);
    }
    public Task<Issue> UpdateIssue(long repositoryId, int issueNumber, IssueUpdate issueUpdate)
    {
        return _gitHubClient.Issue.Update(repositoryId, issueNumber, issueUpdate);
    } 
    public Task<Octokit.User> GetGitHubUser(string login)
    {
        return _gitHubClient.User.Get(login);
    }
    public Task<IReadOnlyList<Octokit.User>> GetAllTeamMembers(long teamId)
    {
        return _gitHubClient.Organization.Team.GetAllMembers(teamId);
    }
    public Task<IReadOnlyList<Team>> GetAllOrgTeams()
    {
        return _gitHubClient.Organization.Team.GetAll(_octokitConfigOptions.UoYTrialsOrgName);
    }
    public Task<IssueComment> AddCommentToIssue(long repositoryId, int issueNumber, string comment)
    {
        return _gitHubClient.Issue.Comment.Create(repositoryId, issueNumber, comment);
    }
    public Task<IReadOnlyList<Label>> RemoveLabelFromIssue(long repositoryId, int issueNumber, string label)
    {
        return _gitHubClient.Issue.Labels.RemoveFromIssue(repositoryId, issueNumber, label);
    }
}