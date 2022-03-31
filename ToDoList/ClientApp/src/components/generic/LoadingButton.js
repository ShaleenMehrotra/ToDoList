import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { Button } from '@material-ui/core';
import CircularProgress from '@mui/material/CircularProgress';
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';

const useStyles = makeStyles(() => ({
    button: {
        marginTop: 8,
        marginBottom: 8,
        width: 120,
    }
}));

export default function LoadingButton(props) {
    const classes = useStyles();

    const { isLoading, loaded, children, type, ...other } = props;

    if (loaded) {
        return (
            <Button type={type} className={classes.button} {...other}>
                <CheckCircleOutlineIcon />
            </Button>
        );
    } else if (isLoading) {
        return (
            <Button type={type} className={classes.button} {...other}>
                <CircularProgress size={24} />
            </Button>
        );
    } else {
        return (
            <Button type={type} className={classes.button} {...other}>
                {children}
            </Button>
        );
    }
}
