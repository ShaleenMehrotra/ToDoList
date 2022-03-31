import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { useState } from 'react';
import { Grid } from '@material-ui/core';
import { Container, Box } from '@material-ui/core';
import SimpleHeader from "../components/generic/SimpleHeader";
import { deleteTask } from '../services/TaskServices';
import LoadingButton from '../components/generic/LoadingButton';
import TextField from '@material-ui/core/TextField';

const useStyles = makeStyles(() => ({
    container: {
        marginLeft: "5%",
        marginRight: "5%"
    },
    title: {
        marginTop: 20,
        marginBottom: 40,
    }
}));


const DeleteTask = () => {
    const classes = useStyles();

    const [tasksFetched, setTasksFetched] = useState(false);
    const [id, setId] = useState('');
    const [resultMessage, setResultMessage] = useState('');

    const populateTasks = async (e) => {
        e.preventDefault();
        setTasksFetched(true);

        const payload = { id: parseInt(id) };

        try {
            let result = await deleteTask(payload);
            if (result) {
                setTasksFetched(false);
                setResultMessage(result.result.message);
            }
        } catch (error) {
            setTasksFetched(false);
        }
    }

    const handleIdChange = (e) => {
        setId(e.target.value.trim());
    }


    return (
        <Container maxWidth="xl" className={classes.container}>
            <form className='add-form' noValidate autoComplete="off" onSubmit={populateTasks}>
                <Grid container direction={"row"} justifyContent="space-between" alignItems="flex-start">
                    <Grid container item direction={"column"} xs={12} sm={12} md={10} xl={10}>
                        <Box className={classes.title}>
                            <SimpleHeader title="Delete Task" description="This page deletes a task from the database based on the task id." />
                        </Box>
                        <Box justifyContent="space-around" alignItems="flex-start">
                            <TextField style={{ paddingRight: 20 }} id="standard-basic" label="Id" value={id} onChange={handleIdChange} />
                            <LoadingButton isLoading={tasksFetched} variant="contained" type="submit" color="primary" size="medium">Delete</LoadingButton>
                        </Box>
                    </Grid>
                    <Grid container item xs={12} sm={12} md={6} xl={6} style={{ marginTop: 30 }}>
                        <p><strong>{resultMessage}</strong></p>
                    </Grid>
                </Grid>
            </form >
        </Container>

    )
}

export default DeleteTask;
