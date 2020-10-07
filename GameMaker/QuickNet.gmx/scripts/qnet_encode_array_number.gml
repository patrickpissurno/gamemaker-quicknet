///qnet_encode_array_number(array)

var len = array_length_1d(argument0);
if(len == 0)
{
    return "";
}
else if(len == 1)
{
    return string(argument0);
}
else
{
    var result = string(argument0[0]);
    
    for(var i = 1; i < len; i += 1)
        result += ";" + string(argument0[i]);
        
    return result;
}
