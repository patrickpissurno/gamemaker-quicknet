///get_keyboard_input(current_string, max_length)
var current_string = argument0;
var max_length = argument1;

if(keyboard_check_released(vk_enter))
    return current_string;

var len = string_length(current_string);

if(keyboard_check_released(vk_backspace) && len > 0)
{
    current_string = string_copy(current_string, 1, len - 1);
}
else if(len < max_length && !(keyboard_check(vk_backspace) || keyboard_check(vk_delete)) && keyboard_check_pressed(vk_anykey))
{
    if(keyboard_string != ""){
        current_string = current_string + string_copy(keyboard_string, string_length(keyboard_string), 1);
        keyboard_string = "";
    }
}
    
return current_string;
