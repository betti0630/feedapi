CREATE DATABASE IF NOT EXISTS attrecto_iam;
CREATE DATABASE IF NOT EXISTS attrecto_feed;
GRANT ALL PRIVILEGES ON attrecto_iam.* TO 'myuser'@'%';
GRANT ALL PRIVILEGES ON attrecto_feed.* TO 'myuser'@'%';
FLUSH PRIVILEGES;