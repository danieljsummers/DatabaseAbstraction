/**
 * This file will create the tables required for the Database Abstraction membership and role providers.  Before
 * running this script, do a search-and-replace, replacing $SCHEMA$ with the schema in which the tables should be
 * created (or replacing "$SCHEMA." with "" if you want them created in the public schema).  Also, add applicable
 * GRANTs to these tables once they are created.
 */
create table $SCHEMA$.membership_user (
    user_id                                      varchar(32)   not null,
	username                                     varchar(255)  not null,
	application_name                             varchar(255)  not null,
	email                                        varchar(255)  not null,
	comment                                      varchar(255),
	password                                     varchar(128)  not null,
	password_question                            varchar(255),
	password_answer                              varchar(255),
	is_approved                                  boolean,
	last_activity_date                           timestamp(0),
	last_login_date                              timestamp(0),
	last_password_changed_date                   timestamp(0),
	creation_date                                timestamp(0),
	is_online                                    boolean,
	is_locked_out                                boolean,
	last_locked_out_date                         timestamp(0),
	failed_password_attempt_count                integer,
	failed_password_attempt_window_start         timestamp(0),
	failed_password_answer_attempt_count         integer,
	failed_password_answer_attempt_window_start  timestamp(0),
  primary key (user_id)
);

create unique index idx_membership_user_user_application on $SCHEMA$.membership_user
  (username, application_name);
create index idx_membership_user_application on $SCHEMA$.membership_user
  (application_name);

create table $SCHEMA$.membership_role (
    rolename          varchar(255)  not null,
	application_name  varchar(255)  not null,
  primary key (rolename, application_name)
);

create table $SCHEMA$.membership_user_role (
    username          varchar(255)  not null,
	rolename          varchar(255)  not null,
	application_name  varchar(255)  not null,
  primary key (username, rolename, application_name),
  foreign key fk_membership_role (rolename, application_name)
    references $SCHEMA$.membership_role (rolename, application_name)
);

create index idx_membership_user_role_role_application on $SCHEMA$.membership_user_role
  (rolename, application_name);
create index idx_membership_user_role_application on $SCHEMA$.membership_user_role
  (application_name);