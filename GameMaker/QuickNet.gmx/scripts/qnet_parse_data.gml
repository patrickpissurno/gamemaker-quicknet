///qnet_parse_data(data) => [key, value]

var result;

var i = string_pos('=', argument0);
    
result[0] = string_copy(argument0, 1, i - 1);
result[1] = string_copy(argument0, i + 1, string_length(argument0) - i + 1);

return result;
