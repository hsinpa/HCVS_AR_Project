CREATE TABLE Teacher (
	id VARCHAR(150) PRIMARY KEY,
    account_name NVarchar(100) NOT NULL,
    password VARCHAR(150) NOT NULL
) CHARACTER SET utf8 COLLATE utf8_unicode_ci;

CREATE TABLE C (
	id VARCHAR(150) PRIMARY KEY NOT NULL,
    year INT NOT NULL,
    semester INT NOT NULL,
    grade INT NOT NULL,
    class_id VARCHAR(150) NOT NULL,
   	class_name Varchar(50)
) CHARACTER SET utf8 COLLATE utf8_unicode_ci;

CREATE INDEX ClassRoomIndex 
ON ClassRoom (year, class_id);

CREATE TABLE Student (
    id VARCHAR(150) PRIMARY KEY,
    student_name NVarchar(50),
    class_id VARCHAR(150)
) CHARACTER SET utf8 COLLATE utf8_unicode_ci;

CREATE TABLE ScoreTable (
    id INT PRIMARY KEY AUTO_INCREMENT,
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

INSERT INTO Teacher (id, account_name, password)
VALUES
    ('0905556696', 'Hsinpa', 'hsinpa@gmail.com'),
    ('339939823', 'FakeTeacher_1', 'faket_1@gmail.com'),
    ('493825793', 'FakeTeacher_2', 'faket_2@gmail.com'),
    ('32330021', 'FakeTeacher_3', 'faket_3@gmail.com');

INSERT INTO Student (id,  student_name, class_id)
VALUES
    ('4897347',  'FakeSport_1',  '14343432'),
    ('1234123',  'FakeSport_2',  '14343432'),
    ('545234', 'FakeSport_3', '14343432'),
    ('123412', 'FakeSport_4',  '14343432'),
    ('124125',  'FakeSport_5',  '14343432'),
    ('988894',  'FakeSport_6', '14343432'),
    ('986532',  'FakeSport_7',  '14343432'),
    ('997422',  'FakeSport_8',  '14343432'),
    ('998764', 'FakeSport_9', '14343432'),

    ('763462',  'FakeArt_1', '57838582'),
    ('1237732',  'FakeArt_2', '57838582'),
    ('9746822',  'FakeArt_3', '57838582'),
    ('4939482',  'FakeArt_4',  '57838582'),
    ('1020394', 'FakeArt_5',  '57838582'),
    ('4828290',  'FakeArt_6',  '55783858223'),
    ('5779403', 'FakeArt7',  '57838582'),
    ('684802', 'FakeArt_8',  '57838582'),
    ('7940856',  'FakeArt_9', '57838582'),

    ('14741588',  'FakeMusic_1', '39836729'),
    ('48579302',  'FakeMusic_2', '39836729'),
    ('20148854',  'FakeMusic_3',  '39836729'),
    ('90884733',  'FakeMusic_4',  '39836729'),
    ('39293934',  'FakeMusic_5', '39836729'),
    ('58574949',  'FakeMusic_6',  '39836729'),
    ('10399875',  'FakeMusic_7', '39836729'),
    ('4458744',  'FakeMusic_8',  '39836729'),
    ('39294874', 'FakeMusic_9', '39836729');


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