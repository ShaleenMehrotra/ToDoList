import React from 'react';
import PropTypes from 'prop-types';
import { useTheme } from '@material-ui/core/styles';
import { makeStyles } from '@material-ui/core/styles';
import { useState } from 'react';
import { Grid } from '@material-ui/core';
import { Container, Box } from '@material-ui/core';
import SimpleHeader from "../components/generic/SimpleHeader";
import IconButton from '@material-ui/core/IconButton';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TablePagination from '@material-ui/core/TablePagination';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import FirstPageIcon from '@material-ui/icons/FirstPage';
import KeyboardArrowLeft from '@material-ui/icons/KeyboardArrowLeft';
import KeyboardArrowRight from '@material-ui/icons/KeyboardArrowRight';
import LastPageIcon from '@material-ui/icons/LastPage';
import { getAllTasks } from '../services/TaskServices';
import LoadingButton from '../components/generic/LoadingButton';

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

function TablePaginationActions(props) {
    const theme = useTheme();

    const { count, page, rowsPerPage, onPageChange } = props;

    const handleFirstPageButtonClick = (event) => {
        onPageChange(event, 0);
    };

    const handleBackButtonClick = (event) => {
        onPageChange(event, page - 1);
    };

    const handleNextButtonClick = (event) => {
        onPageChange(event, page + 1);
    };

    const handleLastPageButtonClick = (event) => {
        onPageChange(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
    };

    return (
        <Box sx={{ flexShrink: 0, ml: 2.5 }}>
            <IconButton
                onClick={handleFirstPageButtonClick}
                disabled={page === 0}
                aria-label="first page"
            >
                {theme.direction === 'rtl' ? <LastPageIcon /> : <FirstPageIcon />}
            </IconButton>
            <IconButton
                onClick={handleBackButtonClick}
                disabled={page === 0}
                aria-label="previous page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowRight /> : <KeyboardArrowLeft />}
            </IconButton>
            <IconButton
                onClick={handleNextButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="next page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowLeft /> : <KeyboardArrowRight />}
            </IconButton>
            <IconButton
                onClick={handleLastPageButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="last page"
            >
                {theme.direction === 'rtl' ? <FirstPageIcon /> : <LastPageIcon />}
            </IconButton>
        </Box>
    );
}

TablePaginationActions.propTypes = {
    count: PropTypes.number.isRequired,
    onPageChange: PropTypes.func.isRequired,
    page: PropTypes.number.isRequired,
    rowsPerPage: PropTypes.number.isRequired,
};

const tableColumns = [
    { id: 'id', label: 'Task Id' },
    { id: 'description', label: 'Description' },
    { id: 'createdDate', label: 'Created Date' }
];

const ToDoList = () => {
    const classes = useStyles();
    const minDataLength = 1;

    const [tasks, setTasks] = useState([]);
    const [tasksFetched, setTasksFetched] = useState(false);
    const [showTasks, setShowTasks] = useState(false);

    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);

    // Avoid a layout jump when reaching the last page with empty rows.

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const populateTasks = async (e) => {
        e.preventDefault();
        setTasksFetched(true);
        setShowTasks(false);

        try {
            let result = await getAllTasks();
            if (result) {
                setShowTasks(true);
                setTasksFetched(false);
                setPage(0);
                setTasks(result.tasks);
            }
        } catch (error) {
            setTasksFetched(false);
        }
    }

    return (
        <Container maxWidth="xl" className={classes.container}>
            <form className='add-form' noValidate autoComplete="off" onSubmit={populateTasks}>
                <Grid container direction={"row"} justifyContent="space-between" alignItems="flex-start">
                    <Grid container item direction={"column"} xs={12} sm={12} md={5} xl={5}>
                        <Box className={classes.title}>
                            <SimpleHeader title="Tasks" description="This page displays all the tasks in the todo list on clicking the button below." />
                        </Box>
                        <Box justifyContent="space-around" alignItems="flex-start">
                            <LoadingButton isLoading={tasksFetched} variant="contained" type="submit" color="primary" size="medium">All Tasks</LoadingButton>
                        </Box>
                        <Grid container item style={{ width: '100%', marginTop: '20px' }}>
                            {
                                showTasks &&
                                <Paper style={{ width: '100%', alignItems: 'center' }}>
                                    <TableContainer className="table-container" style={{ maxHeight: 330, overflowX: "hidden" }}>
                                        <Table aria-label="collapsible table" stickyHeader>
                                            {!tasksFetched && tasks.length < minDataLength && <caption>No results found</caption>}
                                            <TableHead>
                                                <TableRow>
                                                    {tableColumns.map((column) => {
                                                        return (
                                                            <TableCell align="center" key={column.id}>{column.label}</TableCell>
                                                        )
                                                    })}
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {(rowsPerPage > 0
                                                    ? tasks.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                                    : tasks
                                                ).map((task, index) => (
                                                    <TableRow style={{ cursor: 'pointer' }} key={index} hover>
                                                        <TableCell align="center">{task.id}</TableCell>
                                                        <TableCell align="center">{task.description}</TableCell>
                                                        <TableCell align="center">{task.createdDate}</TableCell>
                                                    </TableRow>
                                                ))}
                                            </TableBody>
                                        </Table>
                                    </TableContainer>
                                    <TablePagination
                                        rowsPerPageOptions={[5, 10, 25, { label: 'All', value: -1 }]}
                                        component="div"
                                        colSpan={3}
                                        count={tasks.length}
                                        rowsPerPage={rowsPerPage}
                                        page={page}
                                        SelectProps={{
                                            inputProps: {
                                                'aria-label': 'rows per page',
                                            },
                                            native: true,
                                            position: "bottom"
                                        }}
                                        onPageChange={handleChangePage}
                                        onRowsPerPageChange={handleChangeRowsPerPage}
                                        ActionsComponent={TablePaginationActions}
                                    />
                                </Paper>
                            }
                        </Grid>
                    </Grid>
                </Grid>
            </form >
        </Container>

    )
}

export default ToDoList;
