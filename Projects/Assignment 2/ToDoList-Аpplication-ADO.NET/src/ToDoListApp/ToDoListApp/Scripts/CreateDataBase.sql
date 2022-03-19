CREATE TABLE users (
	userId INT IDENTITY(1,1) PRIMARY KEY,
	username VARCHAR(20) UNIQUE,
	password VARCHAR(20),
	firstName VARCHAR(20),
	lastName VARCHAR(20),
	isAdmin BIT,
	creatorId INT FOREIGN KEY REFERENCES users(userId),
	createdAt DATETIME,
	modifierId INT FOREIGN KEY REFERENCES users(userId),
	modifiedAt DATETIME
);
 
CREATE TABLE toDoLists (
	listId INT IDENTITY(1, 1) PRIMARY KEY,
	title VARCHAR(20) UNIQUE,
	creatorId INT FOREIGN KEY REFERENCES users(userId),
	createdAt DATETIME,
	modifierId INT FOREIGN KEY REFERENCES users(userId),
	modifiedAt DATETIME
	);
 
CREATE TABLE tasks(
	taskId INT IDENTITY(1, 1) PRIMARY KEY,
	title VARCHAR(20) UNIQUE,
	description VARCHAR(40),
	isComplete BIT,
	listId INT FOREIGN KEY REFERENCES toDoLists(listId),
	creatorId INT FOREIGN KEY REFERENCES users(userId),
	createdAt DATETIME,
	modifierId INT FOREIGN KEY REFERENCES users(userId),
	modifiedAt DATETIME
	);
 
CREATE TABLE sharedToDolists(
	userId INT FOREIGN KEY REFERENCES users(userId),
	listId INT FOREIGN KEY REFERENCES toDoLists(listId)
	);
 
CREATE TABLE assignedTasks(
	userId INT FOREIGN KEY REFERENCES users(userId),
	taskId INT FOREIGN KEY REFERENCES tasks(taskId)
	);