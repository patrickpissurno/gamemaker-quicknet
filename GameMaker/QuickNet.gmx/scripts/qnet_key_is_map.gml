///qnet_key_is_map(key, map_name) => "" if not map, map's key otherwise

var match = argument1 + '{';
var i = string_pos(match, argument0);
if(i == 0)
    return "";
    
i += string_length(match) - 1;

return string_copy(argument0, i + 1, string_length(argument0) - i - 1);
