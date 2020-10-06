///qnet_decode_array(value)

var length = string_length(argument0);
var start = 0;
var i = 0;
var c;

var array_len = 0;
var array = array_create(0);

if(length < 1)
    return array;

// this code does some nasty repetition, but it is
// intentional, for performance purposes

while(true){
    c = string_char_at(argument0, i + 1);
    
    if(i == length - 1){
        array[array_len] = base64_decode(string_copy(argument0, start + 1, i - start + 1));
        break;
    }
    else if(c == ";"){
        array[array_len] = base64_decode(string_copy(argument0, start + 1, i - start));
        start = i + 1;
        array_len += 1;
    }
    
    i += 1;
}

return array;
