namespace Enums.Enums
{
    public enum EmailTypeEnum
    {
        //#region To Requester (1-99)

        ///// <summary>
        ///// Email to Requester when CR is approved.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestApprovedToRequester = 1,
        ///// <summary>
        ///// Email to Requester when CR is declined with amendments.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestDeclinedWithAmendmentsToRequester = 2,
        ///// <summary>
        ///// Email to Requester when CR is declined and abandoned.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestDeclinedAndAbandonedToRequester = 3,
        ///// <summary>
        ///// Email to Requester when CR is completed.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestCompletedToRequester = 4,

        //#endregion


        //#region To Development Lead/ Code Reviewers (100-199)

        ///// <summary>
        ///// Email to DevLead when CR is submitted initially.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPendingInitialApprovalToDevLead = 100,
        ///// <summary>
        ///// Email to DevLead when CR is re-submitted with changes.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPendingSubsequentApprovalToDevLead = 101,

        //#endregion


        //#region To Development Team (200-299)

        ///// <summary>
        ///// Email to DevTeam when CR is approved and requires dev work.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPendingDevWorkToDevTeam = 200,
        ///// <summary>
        ///// Email to DevTeam when CR dev work is complete and requires a peer-review.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPendingPeerReviewToDevTeam = 201,
        ///// <summary>
        ///// Email to DevTeam when CR is ready for release.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPendingReleaseToDevTeam = 202,

        //#endregion


        //#region To Specific Developer (300-399)

        ///// <summary>
        ///// Email to Developer whom did dev work when it's approved in peer-review.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPeerReviewApprovalToDev = 300,
        ///// <summary>
        ///// Email to Developer whom did dev work when it's declined in peer-review.
        ///// </summary>
        //[IsActive(true)]
        //ChangeRequestPeerReviewDeclinedToDev = 301

        //#endregion
    }
}
