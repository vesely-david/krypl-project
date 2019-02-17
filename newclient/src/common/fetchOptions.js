export const get = () => ({
  method: 'GET',
  headers: {
    'Accept': 'application/json',
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
  post,
}