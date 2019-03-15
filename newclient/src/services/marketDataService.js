import axios from 'axios';


async function getMarketData(){
  const response = await axios.get(`${document.marketApi}/exchanges`, {
    headers: {
      'Accept': 'application/json',
    },
  })
  return response.data;
}

export const marketDataService = {
  getMarketData,
}

// const response = await axios.post(
//   `${document.api}vault/${vaultId}/batch/v2/${id}`,
//   JSON.stringify({
//     base64FilePart: data,
//     partNumber: partNumber + 1,
//     sizeInKB: 0
//   }),
//   {
//     headers: {
//       'Accept': 'application/json',
//       'Content-Type': 'application/json',
//       'Authorization': 'Bearer ' + localStorage.getItem('user_token')
//     },
//     onUploadProgress: progressEvent => onProgress((progressEvent.loaded) / progressEvent.total)
//   }
// )
// return response.data;