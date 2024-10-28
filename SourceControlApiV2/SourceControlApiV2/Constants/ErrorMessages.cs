namespace SourceControlAPI.Constants
{
    public static class CommonErrorMessages
    {
         public const string RequiredName = "Name is required!";
         public const string NameLength = "Invalid name length!";
        public const string RequiredTitle = "Title is required!";
        public const string TitleLength = "Invalid title length!";
        public const string RequiredDescription = "Description is required!";
         public const string LengthDescription = "Invalid description length!";
    }

    public static class UserErrorMessages
    {
        public const string InvalidUsernameLength = "Invalid username length!";
        public const string UsernameRequired = "Username is required!";
        public const string InvalidPasswordLength = "Invalid password length!";
        public const string PasswordRequired = "Password is required!";
        public const string InvalidEmail = "Invalid email!";
        public const string InvalidUserId = "Invalid user ID!";
        public const string EmailRequired = "Email is required!";
        public const string InvalidUsernameOrPassword = "Invalid username or password!";
        public const string UserIdRequired = "User id is required!";
        public const string UserIsNotContributor = "User is not a contributor!";
        public const string UserAddingContributors = "Only the owner can add contributors!";
        public const string UserDeletingRepository = "Only the owner can delete the repository!";
    }

    public static class RepositoryErrorMessages
    {
        public const string RepossitoryIdRequired = "Repository ID is required!";
        public const string VisibilityRequired = "Visibility is required!";
        public const string InvalidRepositoryId = "Invalid repository ID!";
        public const string UserGettingRepositories = "Error getting repositories!";
        public const string ErrorCreatingRepository = "Error creating repository!";
        public const string ErrorAddingContributor = "Error adding contributor!";
        public const string ErrorRemovingContributor = "Error removing contributor!";
    }

    public static class IssueErrorMessages
    {
        public const string TagsRequired = "Tags field is required!";
        public const string TagsLength = "Invalid tags length!";
        public const string StatusRequired = "Status is required!";
        public const string InvalidStatus = "Status can only be Open, Closed or On hold!";
    }
    public static class ModificationErrorMessages
    {
        public const string DifferencesRequired = "File differences are required!";
        public const string DifferencesLength = "Invalid length for file differences!";
        public const string TypeModificationRequired = "Modification type is required!";
        public const string InvalidModificationType = "Modification type can only be Added, Deleted, or Modified!";
    }

    public static class CommitErrorMessages
    {
        public const string CommitIdRequired = "CommitId is required!";
    }
}
