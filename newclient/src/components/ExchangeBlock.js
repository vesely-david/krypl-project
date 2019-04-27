import React from 'react'
// import Asset from './Asset'
import {Table} from 'semantic-ui-react';
import{
  formatBtc
} from '../common/formaters';

import styles from './styles/home.module.scss';

const ExchangeBlock = ({
  exchange,
  currencies
}) => {
  return (
    //TODO : Check if Free > 0; if not => red color with infobox onHover 
    <div className={styles.exchange_block}>
      <h4>{exchange}</h4>
      <Table basic='very' textAlign='center' verticalAlign='middle' selectable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Currency</Table.HeaderCell>
            <Table.HeaderCell>Total</Table.HeaderCell>
            <Table.HeaderCell>Free</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {currencies.map(o =>
            <Table.Row
              key={`${exchange}_${o.currency}`}
              textAlign='center'
            >
              <Table.Cell>{o.currency}</Table.Cell>
              <Table.Cell>{formatBtc(o.taken + o.free)}</Table.Cell>
              <Table.Cell>{formatBtc(o.free)}</Table.Cell>
            </Table.Row>          
          )}
        </Table.Body>
      </Table>
    </div>
  )
}


export default ExchangeBlock