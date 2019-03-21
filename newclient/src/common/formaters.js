import moment from 'moment';

export const formatBtc = (value) => {
  return `${value.toFixed(8)}`;
}
export const formatUsd = (value) => {
  return `${currencyFormater.format(value)}`;
}
export const formatPercentage = (value) => {
  return `${+value.toFixed(2)}`;
}

export const formatDate = (value) => {
  return value ? moment(value).format("MMM Do YY") : '';
}

const currencyFormater = new Intl.NumberFormat('en-US');


export default {
  formatBtc,
  formatUsd,
  formatPercentage,
}