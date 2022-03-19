# Team Project -Final exam
## Scrum team project (3 weeks)

**The Workforce Management API** is a platform for tracking who is in and out of the office and general management of sick leaves, requests for vacations (paid and non-paid) and the respective approvals. 
The system orchestrates the workforce availability, tracking time offs, approvals and sick leaves. 

## 2. Assignment Description

### 2.1. Required Tasks

In order to deliver successful application you will have to present the Database model used, the authentication should be with JWT, Docker image, the code should be covered at least 60% with tests.

**Story 1:** The implemented system provides authentication and authorization service.

**Story 2:** A user can log into the system with her email and a password. When there are no registered users yet, an admin account is provided with default username and password.

**Story 3:** The admin is able to log in using a default user name and password(admin, adminpass) and she is able to register any other user by providing the necessary information and also is able to set a user as an admin.

**Story 4:** If the user has no admin privileges, she is restricted by using the Users Management Endpoint.

**Story 5:** The admin user can edit and delete users as well using the very same endpoint. The information is updated as appropriately.

**Story 6:** **The Teams Management Endpoint** serves as an entry point for all activities for teams’ management. The admin can create, edit and delete information for any team. The changes are made by modifying the respective data. Please consider the dates when doing the changes. For example, the creation date is not to be changed under any circumstances. The modification date is changed after any modification and should be set during initial user creation.

**Story 7:** The admin user can assign any other user to any team in the system.

**Story 8:** **Time Off Requests Management Endpoint** is implemented and gives the opportunity to the admin to edit, create and delete TimeOff requests.

**Story 9:** Any user can **create, edit, and delete** any TimеОff requests created by herself but is not able to do the same with any other user in the system. The user can create three different types of TimeOff requests: paid, unpaid, sick leave. The user gives a description for the request's reason. Every single request requires approval. All the information needed for the approver is saved.

**Story 10:** The user can **edit and delete** a request created by her only. All the given information is saved.

**Story 11:** **All approvers** receive an email notification when the created request is sent.

**Story 12:** The creator of the request receives an email when all approvers respond to the request.

**Story 13:** Users are organized in Teams. Each Team is represented by title, description, team leader and Users that are assigned to this team. Multiple Users can be assigned to a single Team, and a single User can be assigned to multiple Teams. 

**Story 14:** Each User can request time off. TimeOff requests are represented by type (paid, non-paid or sick leave), start date, end date, reason (free text description of the reason for the request) and whether this time off request is approved. 

**Story 15:** When a User submits a sick leave request the system automatically approves it. An email notification should be sent to the team lead andall members in the team.

**Story 16:** When a User submits a time off request then all team leaders, that the User is a part of, have to approve the request before it is marked as approved. If one of those team leads is out-of-office due to time off or sick leave, then their approval is not needed. If all leads of all teams that the User is a part of are out-of-office, then the request is automatically approved. 

**Story 17:** Even if one approver rejects the request the request is set by the system to "rejected" and a notification is sent to the requester and any other approver.

**Story 18:** The statuses that a request can be into: created, awaiting, approved, rejected.


### 2.2. Extra Credit
Add the official days off based on the bulgarian calendar. Take into account when a paid and unpaid holiday is requested. The official holidays should not be taken into account when calculating last day. For the sick leave this will not be valid since it is given until a specific day.

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
	
10. The final exam is tracked in a [Jira](https://www.atlassian.com/software/jira) project. The work is organized in sprints and at the end of each sprint there is to be a retrpospective. Before starting a sprint there is a planning session which will be done using [scrum poker](https://scrumpoker.online).
