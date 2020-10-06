if(!variable_global_exists("text_cursor_counter"))
    global.text_cursor_counter = 0;
    
global.text_cursor_counter = (global.text_cursor_counter + 1) % room_speed;

if(global.text_cursor_counter >= room_speed / 2)
    return "_";
return " ";
