export const formatBtc = (value) => {
  return `${value.toFixed(8)}`;
}
export const formatUsd = (value) => {
  return `${currencyFormater.format(value)}`;
}
export const formatPercentage = (value) => {
  return `${+value.toFixed(2)}`;
}

const currencyFormater = new Intl.NumberFormat('en-US');


export default {
  formatBtc,
  formatUsd,
  formatPercentage,
}