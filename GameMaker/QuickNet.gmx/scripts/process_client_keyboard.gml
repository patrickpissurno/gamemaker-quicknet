///process_client_keyboard(key, subkey, value) -> true if processed, false if not

var index = qnet_key_is_array(argument0, argument1);
if(index == -1)
    return false;

switch(argument1){
    case "keyboard_left":
        global.keyboard_left[index] = real(argument2);
        break;
    case "keyboard_right":
        global.keyboard_right[index] = real(argument2);
        break;
    case "keyboard_up":
        global.keyboard_up[index] = real(argument2);
        break;
    case "keyboard_down":
        global.keyboard_down[index] = real(argument2);
        break;
}
