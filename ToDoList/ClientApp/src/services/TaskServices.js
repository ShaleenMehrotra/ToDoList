export const addTask = async (taskPayload) => {
    const headers = {
        method: 'POST',
        headers: {
            'Content-type': 'application/json',
        }
    }

    taskPayload && (headers.body = JSON.stringify(taskPayload));

    try {
        const result = await fetch(`${"tasks"}`, headers).then(res => res.json());
        return result;
    } catch (error) {
        throw new Error(error.message);
    }
}


export const getAllTasks = async () => {
    const headers = {
        method: 'GET',
        headers: {
            'Content-type': 'application/json',
        }
    }

    try {
        const result = await fetch(`tasks`, headers).then(res => res.json());
        return result;
    } catch (error) {
        throw new Error(error.message);
    }
}
