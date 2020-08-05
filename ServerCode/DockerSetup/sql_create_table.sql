CREATE TABLE Teacher (
	id VARCHAR(150) PRIMARY KEY,
    account_name NVarchar(100) NOT NULL,
    email VARCHAR(150) NOT NULL,
    account_type VARCHAR(10),
   	isValid BIT
);

CREATE TABLE ClassRoom (
	id VARCHAR(150) PRIMARY KEY,
    year INT NOT NULL,
    semester INT NOT NULL,
    grade INT NOT NULL,
    class_id VARCHAR(150) NOT NULL,
   	class_name NVarchar(50),
   	class_type NVarchar(50),
   	class_group NVarchar(50)
);

CREATE INDEX ClassRoomIndex 
ON ClassRoom (year, class_id);

CREATE TABLE Student (
    id VARCHAR(150) PRIMARY KEY,
    year INT NOT NULL,
    semester INT NOT NULL,
    student_name NVarchar(50),
    seat VARCHAR(20),
    class_id VARCHAR(150)
);

CREATE TABLE ScoreTable (
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id VARCHAR(150) NOT NULL,
    mission_id VARCHAR(80) NOT NULL,
    score INT
);
CREATE INDEX StudentIndex
ON  ScoreTable(student_id)

INSERT INTO ClassRoom (id, year, semester, grade, class_id, class_name)
VALUES
    ('212', 108, 1, 1, '14343432', N'體育一真'),
    ('523', 108, 1, 1, '57838582', N'美術一真'),
    ('8742', 108, 1, 1, '39836729', N'管琴樂一真');

INSERT INTO Teacher (id, account_name, email, account_type, isValid)
VALUES
    ('0905556696', 'Hsinpa', 'hsinpa@gmail.com', 'teacher', 1),
    ('339939823', 'FakeTeacher_1', 'faket_1@gmail.com', 'teacher', 1),
    ('493825793', 'FakeTeacher_2', 'faket_2@gmail.com', 'teacher', 1),
    ('32330021', 'FakeTeacher_3', 'faket_3@gmail.com', 'teacher', 0);

INSERT INTO Student (id, year, semester, student_name, seat, class_id)
VALUES
    ('4897347', 108, 1, 'FakeSport_1', '1', '212'),
    ('1234123', 108, 1, 'FakeSport_2', '2', '212'),
    ('545234', 108, 1, 'FakeSport_3', '3', '212'),
    ('123412', 108, 1, 'FakeSport_4', '4', '212'),
    ('124125', 108, 1, 'FakeSport_5', '5', '212'),
    ('988894', 108, 1, 'FakeSport_6', '6', '212'),
    ('986532', 108, 1, 'FakeSport_7', '7', '212'),
    ('997422', 108, 1, 'FakeSport_8', '8', '212'),
    ('998764', 108, 1, 'FakeSport_9', '9', '212'),

    ('763462', 108, 1, 'FakeArt_1', '1', '523'),
    ('1237732', 108, 1, 'FakeArt_2', '2', '523'),
    ('9746822', 108, 1, 'FakeArt_3', '3', '523'),
    ('4939482', 108, 1, 'FakeArt_4', '4', '523'),
    ('1020394', 108, 1, 'FakeArt_5', '5', '523'),
    ('4828290', 108, 1, 'FakeArt_6', '6', '523'),
    ('5779403', 108, 1, 'FakeArt7', '7', '523'),
    ('684802', 108, 1, 'FakeArt_8', '8', '523'),
    ('7940856', 108, 1, 'FakeArt_9', '9', '523'),

    ('14741588', 108, 1, 'FakeMusic_1', '1', '8742'),
    ('48579302', 108, 1, 'FakeMusic_2', '2', '8742'),
    ('20148854', 108, 1, 'FakeMusic_3', '3', '8742'),
    ('90884733', 108, 1, 'FakeMusic_4', '4', '8742'),
    ('39293934', 108, 1, 'FakeMusic_5', '5', '8742'),
    ('58574949', 108, 1, 'FakeMusic_6', '6', '8742'),
    ('10399875', 108, 1, 'FakeMusic_7', '7', '8742'),
    ('4458744', 108, 1, 'FakeMusic_8', '8', '8742'),
    ('39294874', 108, 1, 'FakeMusic_9', '9', '8742');


INSERT INTO ScoreTable (student_id, mission_id, score)
VALUES
    ('4897347', 'A', 1),
    ('4897347', 'B', 2),
    ('4897347', 'C', 1),

    ('124125', 'A', 1),
    ('124125', 'B', 2),
    ('124125', 'D', 1),

    ('997422', 'A', 3),
    ('997422', 'B', 1),
    ('997422', 'D', 5),

    ('9746822', 'E', 3),
    ('9746822', 'B', 1),
    ('9746822', 'D', 5),

    ('763462', 'J', 3),
    ('763462', 'B', 1),
    ('763462', 'D', 5),

    ('5779403', 'I', 3),
    ('5779403', 'B', 1),
    ('5779403', 'D', 5),

    ('14741588', 'J', 3),
    ('14741588', 'B', 1),
    ('14741588', 'D', 5),

    ('10399875', 'A', 3),
    ('10399875', 'F', 1),
    ('10399875', 'D', 5),

    ('20148854', 'J', 3),
    ('20148854', 'G', 1),
    ('20148854', 'D', 5);