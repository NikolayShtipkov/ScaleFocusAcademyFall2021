# SFA DotNet - Midterm exam 2

## Project Management application with ASP.NET Core Identity and IdentityServer4 (1 week) 


## 1. Assignment Goals 
Following the logic of your Midterm 1 assignment you are expected to implement authorization and authentication to your project management application using industry standard protocols and libraries. Using the knowledge and experience you acquired during week 7 Implement JWT bearer token authentication and authorization for the application.

**IMPORTANT NOTE: If your project for Midterm 1 had major issues/missing functionalities noted by your mentor you are supposed to fix them! Not doing so will result in bad grade for this assignment.**


## 2. Assignment Description 

### 2.1 Required Tasks 
#### Authentication
1. The User should be able to login to the Project Management API with his username and password. When the user logs in for a first time and this is the first application execution there should be two users, the administrator and a manager.

2. Username: admin Password: adminpass

3. Username: manager Password: managerpass

4. For a User without administrative privileges the access to the Users Management Endpoints needs to be restricted (exception for the list users - they should be accessible by all authenticated users).

5. For a User without administrative or manager privileges the access to the Teams Management Endpoints needs to be restricted.

6. Login should be implemented using the IdentityServer4 library with JWT bearer tokens.

7. All endpoints should be accessible only by registered users, by providing the token as a standard Authorization header.

Using Roles based authorization and Policy based authorization implement the following endpoints with the proper restrictions.
- Admins should have unrestricted access.

####Users Management
8. Implement the following restrictions for the user management.
- Create User - Accessible only by admin (Role of new user should be provided)
- List user - Accessible only by authenticated users
- Update - Accessible only by admin
- Delete - Accessible only by admin

#### Teams Management
9. Implement the following restrictions for the teams management.
- Create - Accessible only by admin or manager
- List - Accessible only by admin or manager
- Update - Accessible only by admin or manager
- Delete - Accessible only by admin or manager
- Add Team Member- Accessible only by admin or manager (or this could be done with the Update endpoint)

#### Projects Management
10. Implement the following restrictions for the teams management.
- Create - Accessible only by authenticated users
- List - Accessible only by authenticated users
- ListMy - Accessible only by authenticated users (data returned is related to user)
- Update - Owner of the Project
- Delete - Owner of the Project
- Assign team - Owner of the Project

#### Task Management
11. Implement the following restrictions for the task management.
- List - Any owner or team member of the project the Task is in
- Create - Any owner or team member of the project the Task is in
- Update - Any owner or team member of the project the Task is in
- Delete - Any owner or team member of the project the Task is in
- Get - Accessible only by authenticated users
- Reassign - The assignee of the Task (the new Assignee should be a team member)

#### Work Log Management
12. Implement the following restrictions for the work log management.

- Create - Only assignee of the Task
- Get - Any Team member or owner of the Project the Task is in.
- Update - Only assignee of the Task
- Delete - Only assignee of the Task

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
