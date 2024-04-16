ALTER TABLE Movies
ADD CONSTRAINT CHK_Price_NonNegative CHECK (Price >= 0);

ALTER TABLE Movies
ADD CONSTRAINT CHK_EndDate CHECK (EndDate > StartDate);



--SELECT  A.ProfilePicture
--FROM Actors A
--JOIN ActorMovies AM ON A.id = AM.actorId
--JOIN Movies M ON AM.movieId = M.id
--WHERE M.id = 1;

--Select MovieId
--from Movies m 
--join ActorMovies am on am.MovieId = m.Id 
--join Actors a on a.id = am.ActorId
--where a.id= 18;





















--create database Movies_Avenue;

--CREATE TABLE Actors (
--    ActorId INT PRIMARY KEY IDENTITY,
--    FirstName NVARCHAR(50) NOT NULL,
--    LastName NVARCHAR(50) NOT NULL,
--    Bio NVARCHAR(1000),
--    ProfilePicture NVARCHAR(225),
--    News NVARCHAR(1000)
--);

--CREATE TABLE Categories (
--    CategoryId INT PRIMARY KEY IDENTITY,
--    Name VARCHAR(100) NOT NULL,
--);

--CREATE TABLE Cinemas (
--    CinemaId INT PRIMARY KEY IDENTITY,
--    Name VARCHAR(100) NOT NULL,
--    Description NVARCHAR(1000),
--    CinemaLogo NVARCHAR(255),
--    Address NVARCHAR(255)
--);

--CREATE TABLE Movies (
--    MovieId INT PRIMARY KEY IDENTITY,
--    Name NVARCHAR(100) NOT NULL,
--    Description NVARCHAR(1000),
--    Price DECIMAL(10, 2) CHECK (Price >= 0),
--    ImgUrl NVARCHAR(255),
--    TrailerUrl NVARCHAR(1000),
--    StartDate DATETIME not null,
--    EndDate DATETIME not null,
--    MovieStatus Int,
--    CinemaId INT,
--    CategoryId INT,
--    FOREIGN KEY (CinemaId) REFERENCES Cinemas(CinemaId),
--    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
--	CONSTRAINT CK_EndDateAfterStartDate CHECK (EndDate >= StartDate)
--);
--go;

--CREATE TRIGGER UpdateMovieStatus
--ON Movies
--AFTER INSERT, UPDATE
--AS
--BEGIN
--    IF UPDATE(StartDate) OR UPDATE(EndDate)
--    BEGIN
--        UPDATE Movies
--        SET MovieStatus = 
--            CASE 
--                WHEN GETDATE() < inserted.StartDate THEN 0 
--                WHEN GETDATE() > inserted.EndDate THEN 2    
--                ELSE 1                             
--            END
--        FROM inserted
--        WHERE Movies.MovieId = inserted.MovieId
--    END
--END


--CREATE TABLE ActorMovie (
--    ActorId INT,
--    MovieId INT,
--    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId),
--    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId),
--    PRIMARY KEY (ActorId, MovieId)
--);

