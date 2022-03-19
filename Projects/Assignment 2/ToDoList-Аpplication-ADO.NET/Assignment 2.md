# SFA DotNet - 2 Assignment
## ToDo Application add connection to database (1 week)

## 1. Assignment Goals
Using the ToDo application you are expected to implement a database connection in order to store all of your data in MSSQL.

## 2. Assignment Description

>Using the stories from Assignment 1 implement an application that stores all of the required data in an MSSQL database. 
 - The database columns should have proper types for the objects they store. 
 - The database tables should have proper Primary and Foreign Keys configured on the respective columns.
 - Data in the database should be in normalized format following the relational database normalization techniques.
 - Use ADO.NET without an ORM (EntityFramework, NHibernate, Dapper ORM etc.) for the implementation of your database code.
 - Use configuration files to configure your connection strings
 - The Extra Credit (CLI Tool) tasks from Assignment 1 are not included in the scope of this one.

### 2.1 Required Tasks

**Task 1** 
> Create a Database and database tables that will store the data of your ToDo application.

**Task 2** 
>When your application is started for the first time it should be able to create an empty database and tables so that developers working with the code don't have to create the database themselves.
Generating unique and sequential Ids should be done in the database.


### 2.2. Extra Credit
No extra credit is preserved.

## 3. Assignment Grading
In all the assignments, writing quality code that builds without warnings or errors, and then testing the resulting application and iterating until it functions properly is the goal. Here are the most common reasons assignments receive low points:
- Project does not build.
- One or more items in the Required functionalities section was not satisfied.
- A fundamental concept was not understood.
- Project does not build without warnings.
- Code Quality - Your solution is difficult (or impossible) for someone reading the code to understand due to: 
- Code is visually sloppy and hard to read (e.g. indentation is not consistent, etc.).
- No meaningful variable, method and class names 
- Not following C# code style guides 
- Over/under used methods, classes, variables, data structures or code comments.
- Assignment is not submitted as per Assignment Submission section below.


## 4. Assignment Submission

You already have access to your personal ScaleFocus Academy repositories in GitLab. Every Assignment is submitted in a separate folder in that repo, on your master branch. Every folder is named by the assignment name and number -ex: Final Exam.

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

2. Open a bash terminal (CMD for Windows) in your Assignment folder or navigate there with cd Assignment-1/

3. Create a new branch for the feature you're developing git checkout -b login View, where login Views the name of your new branch.

4. Now you need to add all the file you have changed. You can use git add .when you want to add all files in the current folder. Or use git add file.txt you can define specific files you want to push to yor remote repo.

5. Your next step is to commit the changes you have made. You need to use git commit -m "add README" where the message must be meaningful and is describe the exact reason for change;

6. The last step you need to perform is to push your changes to the remote repo by git push -u origin loginView. Pay close attention that master is your main branch and you are not committing to it directly. Pushes are done ONLY against feature branches(branches other than master)!

7. Create a Merge Request and assign your Tutor to it -Open GitLab and navigate to Merge Requests> Create new Merge Request and select your feature branch login Views source and master as target/destination.

8. Your Tutor/Mentor will now review your code. If you have merge request comments, you will need to resolve them before proceeding: 
		
  * Up vote or write something under the comment, acknowledging that you agree with the comment. If there is something you don't understand, now is your time to discuss it by writing under this comment.
  * If everything is clear with the comment, go back to your source code. Make sure you're on your branch, by calling git checkout loginView
  * Do work here that resolves comments
  * Commit as usual(check above).
  * The merge request will be updated with the new code, so your Tutor/Mentor will see your new changes. If there are additional Merge Request comments repeat step 
	
9. When done with all changes you will be allowed to merge your branch with the master branch. Do not forget to mark the branch to be deleted after the merge. Keep in mind that all versions of your code are kept in git and you don't need the branches in your repo.
