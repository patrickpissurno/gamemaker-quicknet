///qnet_key_is_array(key, array_name) => -1 if not array, index otherwise

var match = argument1 + '[';
var i = string_pos(match, argument0);
if(i == 0)
    return -1;
    
i += string_length(match) - 1;

return real(string_copy(argument0, i + 1, string_length(argument0) - i - 1));
