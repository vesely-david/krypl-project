export const get = () => ({
  method: 'GET',
  headers: {
    'Accept': 'application/json',
  }
});

export const authorizedGet = () => ({
  method: 'GET',
  headers: {
    'Accept': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  }
});

export const post = () => ({
  method: 'POST',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
  },
});

export default{
  get,
  authorizedGet,
  post,
}