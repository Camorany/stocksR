import { TestBed } from '@angular/core/testing';

import { SignalRClient } from './signal-rclient';

describe('SignalRClient', () => {
  let service: SignalRClient;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalRClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
