insert into users (name, email, password, phone_number, is_admin)
values ('test', 'test@test.com', 'password', '89999999999', false);

insert into types_of_event (name)
values ('birthday'),
       ('marriage'),
       ('corporative'),
       ('other');

insert into templates (name, price, file_path, preview_path, id_type_of_event)
values ('Свадьба 1', 2000, 'D:\\path',
        'https://res.cloudinary.com/dygn7je7z/image/upload/v1672074167/merriage_ynriy4.jpg', 2),
       ('День рождения 1', 2000, 'D:\\path',
        'https://res.cloudinary.com/dygn7je7z/image/upload/v1672074167/birthday_pxjjhm.jpg', 1),
       ('Корпоратив 1', 2000, 'D:\\path',
        'https://res.cloudinary.com/dygn7je7z/image/upload/v1672074167/corporative_ft6uzq.jpg', 3),
       ('Своё приглашение', 4000, 'D:\\path',
        'https://res.cloudinary.com/dygn7je7z/image/upload/v1672074166/my_invitation_vnwm9m.jpg', 3);