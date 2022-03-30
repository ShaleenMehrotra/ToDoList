import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { useState } from 'react';
import { Grid } from '@material-ui/core';
import { Container, Box } from '@material-ui/core';
import SimpleHeader from "../components/generic/SimpleHeader";
import { addTask } from '../services/TaskServices';
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


const AddTask = () => {
    const classes = useStyles();

    const [tasksFetched, setTasksFetched] = useState(false);
    const [id, setId] = useState('');
    const [description, setDescription] = useState('');
    const [resultMessage, setResultMessage] = useState('');

    const populateTasks = async (e) => {
        e.preventDefault();
        setTasksFetched(true);

        const payload = { id: id, description: description };

        try {
            let result = await addTask(payload);
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

    const handleDescriptionChange = (e) => {
        setDescription(e.target.value.trim());
    }

    return (
        <Container maxWidth="xl" className={classes.container}>
            <form className='add-form' noValidate autoComplete="off" onSubmit={populateTasks}>
                <Grid container direction={"column"} justifyContent="space-between" alignItems="flex-start">
                    <Grid container item xs={12} sm={12} md={5} xl={5}>
                        <Box className={classes.title}>
                            <SimpleHeader title="Add Task" description="This page adds a new task to the database." />
                        </Box>
                        <Box justifyContent="space-around" alignItems="flex-start">
                            <TextField style={{ paddingRight: 20 }} id="standard-basic" label="Id" value={id} onChange={handleIdChange} />
                            <TextField style={{ paddingRight: 2 }} id="standard-basic" label="Description" value={description} onChange={handleDescriptionChange} />
                            <LoadingButton isLoading={tasksFetched} variant="contained" type="submit" color="primary" size="medium">Add Task</LoadingButton>
                        </Box>
                    </Grid>
                    <Grid container item xs={12} sm={12} md={6} xl={6}>
                        <h3>{resultMessage}</h3>
                    </Grid>
                </Grid>
            </form >
        </Container>

    )
}

export default AddTask;
