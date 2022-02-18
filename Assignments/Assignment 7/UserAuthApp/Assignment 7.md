# SFA DotNet - Assignment 7

## Implement Authentication/Authorization (1 Week)

## 1. Assignment Goals 
The goal of this assignment is to make our application secure and accessible only by users who have credentials.

Take advantage of the authentication authorization mechanisms in ASP and the IdentityServer4 library to add JwtBearer authentication.

### 2.1 Required Tasks 

1. Create a new Web API project that uses Asp.NET Identity with Entity Framework add IdentityServer4 authentication and middlewares. The project should be configured to return a token that can later be used to make request to protected endpoints.

2. Create a Controller that registers new Users and lists existing Users
3. Create another Controller that returns the current DateTime

4. When the app is started for the first time there should be one user already in the database. User name: "admin" Password: "adminpassword"

5. The user should be able to use the token endpoint provided by IdentityServer in order to authenticate.

6. The token should work with the built in Authorization in asp.net instead of having custom code to extract the data from the header.

7. Using the authorization mechanisms in ASP make all actions of the Users controller to be accessible
only by admin users and all other to be accessible by any user that is authenticated.

8. Following the demo application there should be an Authorize button in the swagger UI that allows setting the token.

### 2.2. Extra Credit 

No extra credit is preserved. 

 
 ## 3. Assignment Grading
In all the assignments, writing quality code that builds without warnings or errors, and then testing the resulting application and iterating until it functions properly is the goal. Here are the most common reasons assignments receive low points:
- Project does not build.
- One or more items in the Required functionalities section was not satisfied.
- A fundamental concept was not understood.
- Project does not build without warnings.
- Code Quality -Your solution is difficult (or impossible) for someone reading the code to understand due to: 
  - Code is visually sloppy and hard to read (e.g. indentation is not consistent, etc.).
  - No meaningful variable, method and class name following C# code style guide. Over/under used methods, classes, variables, data structures or code comments.
    - Assignment is not submitted as per Assignment Submission section below.


## 4. Assignment Submission


You already have access to your personal Scalefocus Academy repos in GitLab. Every Assignment is submitted in a separate folder in that repo, on your master branch. Every folder is named by the assignment name and number -ex: Final Exam.


Here is a sample structure of how your master branch of your repo should look like towards the end of your SFA training:
```
.
├── Assignment 1
├── Assignment 2  
├── Assignment 3
├── Assignment 4
├── Midterm 1
├── Assignment 5
├── Midterm 2
├── Assignment 6
├──  └── Workforce-management
├──          └── README.md
├──          └── src
├──              └── WorkforceManagement.sln
├──              └── ConsoleApp
├──                  └── ConsoleApp.csproj
├──                  └── Program.cs
├──                  └── ...
├──              └── ClassLibrary
├──                  └── LibraryClass.cs
├──                  └── ...
├──          └── ...
...
```
Assignments that have not been submitted to the master branch or have incorrect folder structure will not be graded.


<small>How to use Git to submit your assignments for review?<small>


> ⚠️ Make sure you have read the GitLab Reading Materials first, available here.


Let's imagine that the first required task from your assignment is to create a Login View in your project:


1. Make sure you have the latest version of your code;


2. Open a bash terminal (CMD for Windows) in your Assignment folder or navigate there with cd Assignmnet-1/


3. Create a new branch for the feature you're developing git checkout -b loginView, where loginViewis the name of your new branch.


4. Now you need to add all the file you have changed. You can use git add .when you want to add all files in the current folder. Or use git add file.txt you can define specific files you want to push to your remote repo.


5. Your next step is to commit the changes you have made. You need to use git commit -m "add README" where the message must be meaningful and is describe the exact reason for change;


6. The last step you need to perform is to push your changes to the remote repo by git push -u origin loginView. Pay close attention that master is your main branch, and you are not committing to it directly. Pushes are done ONLY against feature branches (branches other than master)!


7. Create a Merge Request and assign your Tutor to it -Open GitLab and navigate to Merge Requests> Create new Merge Request and select your feature branch loginViewas source and master as target/destination.


8. Your Tutor/Mentor will now review your code. If you have merge request comments, you will need to resolve them before proceeding: 
    
  * Up vote or write something under the comment, acknowledging that you agree with the comment. If there is something you do not understand, now is your time to discuss it by writing under this comment.
  * If everything is clear with the comment, go back to your source code. Make sure you're on your branch, by calling git checkout loginView
  * Do work here that resolves comments
  * Commit as usual(check above).
  * The merge request will be updated with the new code, so your Tutor/Mentor will see your new changes. If there are additional Merge Request comments repeat step 
  
9. When done with all changes you will be allowed to merge your branch with the master branch. Do not forget to mark the branch to be deleted after the merge. Keep in mind that all versions of your code are kept in git and you do not need the branches in your repo.
