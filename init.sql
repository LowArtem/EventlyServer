create table users (
	id serial primary key,
	name varchar(100) not null,
	email varchar(100) not null unique,
	password varchar(100) not null,
	phone_number char(11),
	other_communication varchar(300),
	is_admin boolean not null
);

create table types_of_event (
	id serial primary key,
	name varchar(50) not null unique
);

create table templates (
	id serial primary key,
	name varchar(100) not null unique,
	price money not null,
	file_path varchar(100) not null,
	preview_path varchar(100) not null,
	id_type_of_event integer references types_of_event (id) not null
);

create table landing_invitations (
	id serial primary key,
	name varchar(100) not null unique,
	status varchar(50) not null,
	link varchar(100) unique,
	start_date date not null,
	finish_date date not null,
	id_client integer references users (id) not null,
	id_template integer references templates (id) not null
);

create table guests (
	id serial primary key,
	name varchar(100) not null,
	phone_number char(11) not null,
	id_landing_invitation integer not null,
	foreign key (id_landing_invitation) references landing_invitations (id) on delete cascade
);


create or replace 
function guests_insert() RETURNS trigger AS
$body$
begin
	if (select count(id)
		from guests
		where id_landing_invitation = new.id_landing_invitation
		and phone_number = new.phone_number) > 0
	then raise exception 'Гость с таким номером уже откликнулся на это приглашение.';
	else return new;
	end if;
end
$body$
language plpgsql;

create trigger instead_guests_insert
before insert 
on guests
for each row
execute procedure guests_insert();