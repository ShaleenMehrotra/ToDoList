import React from 'react';
import { Typography, Box } from "@material-ui/core";

const SimpleHeader = ({ title, description }) => {
    return (
        <Box style={{ margin: 0 }}>
            <Typography style={{ fontSize: '2rem', lineHeight: '56px', fontWeight: '300' }}>{title}</Typography>
            <Typography style={{ fontSize: '1.2rem', lineHeight: '36px', fontWeight: '300' }} className="text-highlighted">{description}</Typography>
        </Box>
    )
}

export default SimpleHeader;