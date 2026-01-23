shader_type canvas_item;

// Color replacement shader for frosh skin tones and shirt colors
// Replaces pixels matching replace_color with substitute_color

uniform vec3 replace_color : source_color = vec3(0.0, 0.0, 0.0);
uniform vec3 substitute_color : source_color = vec3(0.0, 0.0, 0.0);
uniform float tolerance : hint_range(0.0, 0.1) = 0.02;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);

    // Check if this pixel matches the replace color (within tolerance)
    vec3 diff = abs(tex_color.rgb - replace_color);
    float max_diff = max(max(diff.r, diff.g), diff.b);

    if (max_diff < tolerance) {
        // Replace the color while preserving alpha
        COLOR = vec4(substitute_color, tex_color.a);
    } else {
        COLOR = tex_color;
    }
}
