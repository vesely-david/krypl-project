import React from 'react';
import { Segment, Button } from 'semantic-ui-react';
import EditApiKeyModal from '../../containers/modals/EditApiKeyModal';
import DeleteApiKeyModal from '../../containers/modals/DeleteApiKeyModal';

import styles from '../styles/account.module.scss';

export default ({
  apiKeyId,
  exchangeId,
  exchangeName,
  apiKey,
  onDelete,
  onEdit,
}) => {
  return(
    <Segment raised className={styles.api_key}>
      <div className={styles.flex}>
        <h3>{exchangeName}</h3>
        <h6>{apiKey}</h6>
      </div>
      <div>
        <Button.Group>
          <EditApiKeyModal 
            exchangeName={exchangeName} 
            newKey={apiKeyId === null} 
            onClick={(values) => onEdit({...values, exchangeId})}
          />
          {apiKeyId !== null && (
            <React.Fragment>
              <Button.Or />
              <DeleteApiKeyModal onClick={() => onDelete(apiKeyId)} />
            </React.Fragment>
          )
          }
        </Button.Group>
      </div>
    </Segment>
  )
}