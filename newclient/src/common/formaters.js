import moment from 'moment';

export const formatBtc = (value) => {
  return value != null ? `${value.toFixed(8)}` : '';
}
export const formatUsd = (value) => {
  return value != null ? `${currencyFormater.format(value)}` : '';
}
export const formatPercentage = (value) => {
  return value != null ? `${+value.toFixed(2)}` : '';
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