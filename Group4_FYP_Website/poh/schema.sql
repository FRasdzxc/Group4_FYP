DROP TABLE IF EXISTS play_sessions;

CREATE TABLE play_sessions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  start_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  steps_taken INTEGER NOT NULL,
  damage_given REAL NOT NULL,
  damage_taken REAL NOT NULL,
  weapon_usage INTEGER NOT NULL,
  ability_usage INTEGER NOT NULL,
  mobs_killed INTEGER NOT NULL,
  weapon_usage_detail TEXT,
  ability_usage_detail TEXT,
  mob_killed_detail TEXT
);
