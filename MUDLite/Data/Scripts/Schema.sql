CREATE TABLE IF NOT EXISTS Tbl_Creatures (
	CR_Key INTEGER PRIMARY KEY AUTOINCREMENT,
	CR_Name TEXT NOT NULL,
	CR_Description TEXT NOT NULL,
	CR_Health INTEGER NOT NULL,
	CR_Attack INTEGER NOT NULL,
	CR_Defense INTEGER NOT NULL,
	CR_Evasion INTEGER NOT NULL,
             CR_Room INTEGER NULL,
	FOREIGN KEY(CR_Room) REFERENCES Tbl_Rooms (RM_Key)
);

CREATE TABLE IF NOT EXISTS Tbl_Rooms (
	RM_Key INTEGER PRIMARY KEY AUTOINCREMENT,
	RM_Name TEXT NOT NULL,
	RM_Description TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Tbl_RoomExits (
	EX_Key INTEGER PRIMARY KEY AUTOINCREMENT,
	EX_Name TEXT NOT NULL,
	EX_From_RM_Key INTEGER NOT NULL,
	EX_To_RM_Key INTEGER NOT NULL,
	FOREIGN KEY(EX_From_RM_Key) REFERENCES Tbl_Rooms (RM_Key),
	FOREIGN KEY(EX_To_RM_Key) REFERENCES Tbl_Rooms (RM_Key),
             UNIQUE(EX_Name, EX_From_RM_Key)
);

CREATE TABLE IF NOT EXISTS Tbl_Users (
	US_Id TEXT PRIMARY KEY,
	US_Name TEXT NOT NULL,
	US_HashedPassword TEXT NOT NULL
) WITHOUT ROWID;

CREATE TABLE IF NOT EXISTS Tbl_Roles (
	RL_Key INTEGER PRIMARY KEY,
	RL_Name TEXT NOT NULL UNIQUE
) WITHOUT ROWID;


CREATE TABLE IF NOT EXISTS Tbl_PlayerCharacters (
    PC_Key INTEGER PRIMARY KEY AUTOINCREMENT,
    PC_CR_Key INTEGER NOT NULL,    
    PC_US_Id TEXT NOT NULL,
    PC_RL_Key int NOT NULL DEFAULT(1),
    FOREIGN KEY(PC_CR_Key) REFERENCES Tbl_Creatures (CR_Key),
    FOREIGN KEY(PC_US_Id) REFERENCES Tbl_Users (US_Id)
);